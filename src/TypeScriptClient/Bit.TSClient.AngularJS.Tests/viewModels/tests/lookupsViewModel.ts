﻿module Bit.Tests.ViewModels {

    @ComponentDependency({ name: "LookupsViewModel", templateUrl: "|Bit|/Bit.TSClient.AngularJS.Tests/views/tests/lookupsView.html" })
    export class LookupsViewModel {

        public constructor(@Inject("EntityContextProvider") public entityContextProvider: Contracts.IEntityContextProvider) {
        }

        public product = new BitTestsModel.ProductDto();

        public offlineCountriesDS: kendo.data.DataSource;
        public onlineCountriesDS: kendo.data.DataSource;
        public localCountriesDS: kendo.data.DataSource;

        @Command()
        public async $onInit(): Promise<void> {
            let onlineContext = await this.entityContextProvider.getContext<TestContext>("Test");
            let offlineContext = await this.entityContextProvider.getContext<TestContext>("Test", { isOffline: true });
            let localData = await onlineContext.countries.getAllCountries().toArray();
            if ((await offlineContext.countries.count()) == 0) {
                offlineContext.countries.addMany(localData);
                await offlineContext.saveChanges();
            }
            this.localCountriesDS = localData.toQueryable(BitTestsModel.CountryDto).asKendoDataSource();
            this.offlineCountriesDS = offlineContext.countries.asKendoDataSource();
            this.onlineCountriesDS = onlineContext.countries.getAllCountries().asKendoDataSource();
        }
    }
}
